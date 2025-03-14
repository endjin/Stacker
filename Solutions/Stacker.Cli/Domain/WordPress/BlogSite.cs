// <copyright file="BlogSite.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Stacker.Cli.Domain.WordPress;

public class BlogSite
{
    private static readonly XNamespace WordpressNamespace = "http://wordpress.org/export/1.2/";
    private static readonly XNamespace DublinCoreNamespace = "http://purl.org/dc/elements/1.1/";
    private static readonly XNamespace ContentNamespace = "http://purl.org/rss/1.0/modules/content/";
    private static readonly XNamespace ExcerptNamespace = "http://wordpress.org/export/1.2/excerpt/";

    private XElement channelElement;

    public BlogSite(XDocument doc)
    {
        this.Authors = [];
        this.Categories = [];
        this.Tags = [];
        this.Attachments = [];

        this.InitializeChannelElement(doc);

        if (this.channelElement == null)
        {
            throw new XmlException("Missing channel element.");
        }

        this.Initialize();
    }

    public string Title { get; set; }

    public string Description { get; set; }

    public IEnumerable<Author> Authors { get; set; }

    public IEnumerable<Category> Categories { get; set; }

    public IEnumerable<Tag> Tags { get; set; }

    public IEnumerable<Attachment> Attachments { get; set; }

    public IEnumerable<Post> GetAllPosts()
    {
        return this.channelElement.Elements("item")
            .Where(e => this.IsPostItem(e) && this.IsPublishedPost(e))
            .Select(this.ParsePostElement);
    }

    public IEnumerable<Post> GetAllPostsInAllPublicationStates()
    {
        return this.channelElement.Elements("item")
            .Where(e => this.IsPostItem(e) && this.IsPublishedOrDraftPost(e))
            .Select(this.ParsePostElement);
    }

    private void InitializeChannelElement(XDocument document)
    {
        XElement rssRootElement = document.Root;
        if (rssRootElement == null)
        {
            throw new XmlException("No document root.");
        }

        this.channelElement = rssRootElement.Element("channel");
    }

    private void Initialize()
    {
        this.InitializeTitle();
        this.InitializeDescription();
        this.InitializeAuthors();
        this.InitializeCategories();
        this.InitializeTags();
        this.InitializeAttachments();
    }

    private void InitializeTitle()
    {
        this.Title = this.GetBasicProperty("title");
    }

    private void InitializeDescription()
    {
        this.Description = this.GetBasicProperty("description");
    }

    private string GetBasicProperty(string elementName)
    {
        XElement element = this.channelElement.Element(elementName);
        if (element == null)
        {
            throw new XmlException($"Missing {elementName}.");
        }

        return element.Value;
    }

    private void InitializeAuthors()
    {
        this.Authors = this.channelElement.Descendants(WordpressNamespace + "author")
            .Select(this.ParseAuthorElement);
    }

    private Author ParseAuthorElement(XElement authorElement)
    {
        XElement authorIdElement = authorElement.Element(WordpressNamespace + "author_id");
        XElement authorUsernameElement = authorElement.Element(WordpressNamespace + "author_login");
        XElement authorEmailElement = authorElement.Element(WordpressNamespace + "author_email");
        XElement authorDisplayNameElement = authorElement.Element(WordpressNamespace + "author_display_name");

        if (authorIdElement == null || authorUsernameElement == null || authorEmailElement == null || authorDisplayNameElement == null)
        {
            throw new XmlException("Unable to parse malformed author.");
        }

        return new()
        {
            Id = authorIdElement.Value,
            Username = authorUsernameElement.Value,
            Email = authorEmailElement.Value,
            DisplayName = authorDisplayNameElement.Value,
        };
    }

    private void InitializeCategories()
    {
        this.Categories = this.channelElement.Descendants(WordpressNamespace + "category").Select(this.ParseCategoryElement);
    }

    private Category ParseCategoryElement(XElement categoryElement)
    {
        XElement categoryIdElement = categoryElement.Element(WordpressNamespace + "term_id");
        XElement categoryNameElement = categoryElement.Element(WordpressNamespace + "cat_name");
        XElement categorySlugElement = categoryElement.Element(WordpressNamespace + "category_nicename");

        if (categoryIdElement == null || categoryNameElement == null || categorySlugElement == null)
        {
            throw new XmlException("Unable to parse malformed category.");
        }

        return new()
        {
            Id = categoryIdElement.Value,
            Name = categoryNameElement.Value,
            Slug = categorySlugElement.Value,
        };
    }

    private void InitializeTags()
    {
        this.Tags = this.channelElement.Descendants(WordpressNamespace + "tag").Select(this.ParseTagElement);
    }

    private Tag ParseTagElement(XElement tagElement)
    {
        XElement tagIdElement = tagElement.Element(WordpressNamespace + "term_id");
        XElement tagNameElement = tagElement.Element(WordpressNamespace + "tag_name");
        XElement tagSlugElement = tagElement.Element(WordpressNamespace + "tag_slug");

        if (tagIdElement == null || tagSlugElement == null)
        {
            throw new XmlException("Unable to parse malformed category.");
        }

        return new()
        {
            Id = tagIdElement?.Value,
            Name = tagNameElement?.Value,
            Slug = tagSlugElement?.Value,
        };
    }

    private void InitializeAttachments()
    {
        this.Attachments = this.channelElement.Elements("item")
            .Where(e => this.IsAttachmentItem(e))
            .Select(this.ParseAttachmentElement);
    }

    private bool IsPostItem(XElement itemElement)
    {
        return itemElement?.Element(WordpressNamespace + "post_type")?.Value == "post";
    }

    private bool IsAttachmentItem(XElement itemElement)
    {
        return itemElement?.Element(WordpressNamespace + "post_type")?.Value == "attachment";
    }

    private bool IsPublishedPost(XElement itemElement)
    {
        return itemElement?.Element(WordpressNamespace + "status")?.Value == "publish";
    }

    private bool IsPublishedOrDraftPost(XElement itemElement)
    {
        return itemElement?.Element(WordpressNamespace + "status")?.Value == "publish" || itemElement?.Element(WordpressNamespace + "status")?.Value == "draft";
    }

    private Attachment ParseAttachmentElement(XElement attachmentElement)
    {
        XElement attachmentIdElement = attachmentElement.Element(WordpressNamespace + "post_id");
        XElement attachmentPostIdElement = attachmentElement.Element(WordpressNamespace + "post_parent");
        XElement attachmentTitleElement = attachmentElement.Element("title");
        XElement attachmentUrlElement = attachmentElement.Element(WordpressNamespace + "attachment_url");

        IEnumerable<XElement> postMetaElements = attachmentElement.Elements(WordpressNamespace + "postmeta");
        XElement metaValueElement = postMetaElements.Elements(WordpressNamespace + "meta_value").FirstOrDefault(x => ((XElement)x.PreviousNode).Value == "_wp_attached_file");

        if (attachmentIdElement == null ||
            attachmentTitleElement == null ||
            attachmentUrlElement == null)
        {
            throw new XmlException("Unable to parse malformed attachment.");
        }

        return new Attachment
        {
            Id = attachmentIdElement?.Value,
            Path = metaValueElement?.Value,
            PostId = attachmentPostIdElement?.Value,
            Title = attachmentTitleElement?.Value,
            Url = attachmentUrlElement?.Value,
        };
    }

    private Post ParsePostElement(XElement postElement)
    {
        XElement postTitleElement = postElement.Element("title");
        XElement postLinkElement = postElement.Element("link");
        XElement postUsernameElement = postElement.Element(DublinCoreNamespace + "creator");
        XElement postBodyElement = postElement.Element(ContentNamespace + "encoded");
        XElement postPublishedAtUtcElement = postElement.Element(WordpressNamespace + "post_date_gmt");
        XElement postSlugElement = postElement.Element(WordpressNamespace + "post_name");
        XElement postPostIdElement = postElement.Element(WordpressNamespace + "post_id");
        XElement postStatusElement = postElement.Element(WordpressNamespace + "status");

        if (postTitleElement == null ||
            postUsernameElement == null ||
            postBodyElement == null ||
            postPublishedAtUtcElement == null ||
            postSlugElement == null)
        {
            throw new XmlException("Unable to parse malformed post.");
        }

        XElement postExcerptElement = postElement.Element(ExcerptNamespace + "encoded");

        if (!DateTimeOffset.TryParse(postPublishedAtUtcElement.Value, out DateTimeOffset publicationData))
        {
            publicationData = DateTimeOffset.MaxValue;
        }

        Post post = new()
        {
            Author = this.GetAuthorByUsername(postUsernameElement.Value),
            Body = postBodyElement.Value,
            Excerpt = postExcerptElement?.Value,
            Id = postPostIdElement?.Value,
            Link = postLinkElement?.Value,
            PublishedAtUtc = publicationData,
            Status = postStatusElement?.Value,
            Slug = postSlugElement.Value,
            Title = postTitleElement.Value,
        };

        post.Attachments = this.GetAttachmentsByPostId(post.Id);

        List<Category> categories = [];
        List<Tag> tags = [];
        Dictionary<string, string> metaData = new();

        foreach (XElement wpCategory in postElement.Elements("category"))
        {
            XAttribute domainAttribute = wpCategory.Attribute("domain");
            if (domainAttribute == null)
            {
                throw new XmlException("Unable to parse malformed wordpress categorization.");
            }

            if (domainAttribute.Value == "category")
            {
                string categorySlug = wpCategory.Attribute("nicename")?.Value;
                Category category = this.GetCategoryBySlug(categorySlug);
                categories.Add(category);
            }
            else if (domainAttribute.Value == "post_tag")
            {
                string tagSlug = wpCategory.Attribute("nicename")?.Value;
                Tag tag = this.GetTagBySlug(tagSlug);
                tags.Add(tag);
            }
        }

        post.Categories = categories;
        post.Tags = tags;

        IEnumerable<XElement> postMetaElements = postElement.Elements(WordpressNamespace + "postmeta");
        foreach (XElement postMeta in postMetaElements)
        {
            XElement metaKeyElement = postMeta.Element(WordpressNamespace + "meta_key");

            // this.ParseSsoMetaData(metaKeyElement, postMeta, metaData);
            this.ParseFeaturedImage(metaKeyElement, postMeta, post);
            this.ParseStackerPromote(metaKeyElement, post);
        }

        post.MetaData = metaData;

        return post;
    }

    private void ParseFeaturedImage(XElement metaKeyElement, XElement postMeta, Post post)
    {
        if (metaKeyElement?.Value == "_thumbnail_id")
        {
            XElement metaValueElement = postMeta.Element(WordpressNamespace + "meta_value");
            string attachmentId = metaValueElement?.Value;
            post.FeaturedImage = this.GetAttachmentById(attachmentId);
        }
    }

    private void ParseStackerPromote(XElement metaKeyElement, Post post)
    {
        try
        {
            if (metaKeyElement?.Value == "stacker_promote")
            {
                XElement promoteElement = metaKeyElement.NextNode as XElement;

                if (bool.TryParse(promoteElement?.Value, out bool promote))
                {
                    post.Promote = promote;
                }
            }

            if (metaKeyElement?.Value == "stacker_promote_until")
            {
                XElement promoteUntilElement = metaKeyElement.NextNode as XElement;

                post.PromoteUntil = DateTimeOffset.TryParse(promoteUntilElement?.Value, out DateTimeOffset promoteUntil) ? promoteUntil : DateTimeOffset.MaxValue;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void ParseSsoMetaData(XElement metaKeyElement, XElement postMeta, Dictionary<string, string> metaData)
    {
        /*
        <wp:postmeta>
            <wp:meta_key><![CDATA[_wpsso_meta]]></wp:meta_key>
            <wp:meta_value><![CDATA[a:4:{s:7:"og_desc";s:278:"TLDR; There is a lot of hype around AI &amp; ML. Here's an example of using ML.NET &amp; Azure Functions to deliver a series of micro-optimisations, to automate a series of 1 second tasks. Applied to business processes this is what the 4th Industrial Revolution could look like.";s:15:"options_version";s:24:"-wpsso699pro-wpssoum4std";s:24:"plugin_wpsso_opt_version";s:3:"699";s:26:"plugin_wpssoum_opt_version";s:1:"3";}]]></wp:meta_value>
        </wp:postmeta>
        */
        if (metaKeyElement?.Value == "_wpsso_meta")
        {
            XElement metaValueElement = postMeta.Element(WordpressNamespace + "meta_value");
            string rawMetaData = metaValueElement?.Value;

            rawMetaData = Regex.Replace(rawMetaData, @"(?<prefix>^\w:\d+:)", string.Empty);
            rawMetaData = Regex.Replace(rawMetaData, @"{(?<first>\w:\d{1,3}:)", "{");
            rawMetaData = Regex.Replace(rawMetaData, "(?<trailing>;)(?:})", "}");

            Regex regexp = new Regex(@"(?<separators>;\w:\d+?:)(?:"")");
            MatchCollection matches = regexp.Matches(rawMetaData);
            int itemMatchIndex = 0;

            foreach (Match match in matches.OrderByDescending(m => m.Index))
            {
                Group group = match.Groups["separators"];

                rawMetaData = rawMetaData.Remove(@group.Index, @group.Value.Length);
                rawMetaData = rawMetaData.Insert(@group.Index, itemMatchIndex % 2 == 0 ? ":" : ",");

                itemMatchIndex++;
            }

            regexp = new(@"(?::""(?<value>.*?)(?="",|""}))");
            matches = regexp.Matches(rawMetaData);

            foreach (Match match in matches.OrderByDescending(m => m.Index))
            {
                Group group = match.Groups["value"];

                rawMetaData = rawMetaData.Remove(@group.Index, @group.Value.Length);
                rawMetaData = rawMetaData.Insert(@group.Index, HttpUtility.HtmlEncode(@group.Value));
            }

            try
            {
                Dictionary<string, string> data = JsonSerializer.Deserialize<Dictionary<string, string>>(rawMetaData);

                foreach ((string key, string value) in data)
                {
                    metaData.Add(key, value);
                }
            }
            catch (Exception exception)
            {
                throw new AggregateException(rawMetaData, exception);
            }
        }

        /*
        <wp:postmeta xmlns:wp="http://wordpress.org/export/1.2/">
          <wp:meta_key><![CDATA[_wpsso_head_info_og_img_thumb]]></wp:meta_key>
          <wp:meta_value><![CDATA[<div class="preview_img" style="background-image:url(https://blogs.endjin.com/wp-content/uploads/2014/09/Troubleshooting-Twilio-with-New-RelicA-P1-1024px-150x150.png);"></div>]]></wp:meta_value>
        </wp:postmeta>
        */
        /*if (metaKeyElement?.Value == "_wpsso_head_info_og_img_thumb")
        {
            var metaValueElement = postMeta.Element(WordpressNamespace + "meta_value");
            string rawMetaData = metaValueElement?.Value;

            var regexp = new Regex(@"\((?<url>.*?)\)");
            var match = regexp.Match(rawMetaData);

            if (match.Groups.ContainsKey("url"))
            {
                var url = match.Groups["url"];
            }
        }*/
    }

    private Author GetAuthorByUsername(string username)
    {
        return this.Authors.FirstOrDefault(a => a.Username == username);
    }

    private Category GetCategoryBySlug(string categorySlug)
    {
        return this.Categories.FirstOrDefault(c => c.Slug == categorySlug);
    }

    private Tag GetTagBySlug(string tagSlug)
    {
        return this.Tags.FirstOrDefault(t => t.Slug == tagSlug);
    }

    private Attachment GetAttachmentById(string attachmentId)
    {
        return this.Attachments.FirstOrDefault(a => a.Id == attachmentId);
    }

    private IEnumerable<Attachment> GetAttachmentsByPostId(string postId)
    {
        return this.Attachments.Where(a => a.PostId == postId);
    }
}