Feature: Convert WordPress XML Export to Twitter

Scenario: Convert a WordPress XML Export File into Tweets
	Given I have a WordPress Export called called 'endjinblog.WordPress.2020-02-10.xml'
	And I want to export it to a file called 'Tweets.txt'
	When I ask the tool to convert the export file
	Then The 'Tweets.txt' file should contain the following items:
	| Rising Stars - Cloud Apprentice & Apprentice Engineer of the Year by @HowardvRooijen https://blogs.endjin.com/2019/10/rising-stars-cloud-apprentice-apprentice-engineer-of-the-year/ |
	| Endjin is a Snowflake Partner by @HowardvRooijen #Snowflake https://blogs.endjin.com/2019/05/endjin-is-a-snowflake-partner/                                                          |
