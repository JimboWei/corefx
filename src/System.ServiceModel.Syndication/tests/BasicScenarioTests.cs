﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Syndication;
using System.Xml;
using System.IO;
using Xunit;

namespace System.ServiceModel.Syndication.Tests
{
    public static class BasicScenarioTests
    {
        [Fact]
        public static void SyndicationFeed_CreateNewFeed()
        {
            string filePath = Path.GetTempFileName();

            try
            {
                // *** SETUP *** \\
                SyndicationFeed sf = new SyndicationFeed("First feed on .net core ever!!", "This is the first feed on .net core ever!", new Uri("https://github.com/dotnet/wcf"));
                Assert.True(sf != null);

                XmlWriter xmlw = XmlWriter.Create(filePath);
                Rss20FeedFormatter rssf = new Rss20FeedFormatter(sf);

                // *** EXECUTE *** \\
                rssf.WriteTo(xmlw);
                xmlw.Close();

                // *** VALIDATE *** \\
                Assert.True(File.Exists(filePath));
            }
            finally
            {
                // *** CLEANUP *** \\
                File.Delete(filePath);
            }
        }

        [Fact]
        public static void SyndicationFeed_Load_Write_RSS_Feed()
        {
            string path = Path.GetTempFileName();

            try
            {
                // *** SETUP *** \\\
                XmlReader xmlr = XmlReader.Create(@"SimpleRssFeed.xml");
                SyndicationFeed sf = SyndicationFeed.Load(xmlr);
                Assert.True(sf != null);

                // *** EXECUTE *** \\
                //Write the same feed that was read.
                XmlWriter xmlw = XmlWriter.Create(path);
                Rss20FeedFormatter atomFeed = new Rss20FeedFormatter(sf);
                atomFeed.WriteTo(xmlw);
                xmlw.Close();

                // *** VALIDATE *** \\
                Assert.True(File.Exists(path));
            }
            finally
            {
                // *** CLEANUP *** \\
                File.Delete(path);
            }
        }

        [Fact]
        public static void SyndicationFeed_Load_Write_RSS_Feed_()
        {
            string path = Path.GetTempFileName();

            try
            {
                // *** SETUP *** \\\
                XmlReaderSettings settingsReader = new XmlReaderSettings();
                XmlReader xmlr = XmlReader.Create(@"rssSpecExample.xml", settingsReader);
                SyndicationFeed sf = SyndicationFeed.Load(xmlr);
                Assert.True(sf != null);

                // *** EXECUTE *** \\
                //Write the same feed that was read.
                XmlWriterSettings settingsWriter = new XmlWriterSettings();
                XmlWriter xmlw = XmlWriter.Create(path, settingsWriter);
                Rss20FeedFormatter atomFeed = new Rss20FeedFormatter(sf);
                atomFeed.WriteTo(xmlw);

                xmlw.Close();

                // *** VALIDATE *** \\
                Assert.True(File.Exists(path));
            }
            finally
            {
                // *** CLEANUP *** \\
                File.Delete(path);
            }
        }

        [Fact]
        public static void SyndicationFeed_Load_Write_Atom_Feed()
        {
            string path = Path.GetTempFileName();

            try
            {
                // *** SETUP *** \\\
                XmlReaderSettings setting = new XmlReaderSettings();
                XmlReader xmlr = XmlReader.Create(@"SimpleAtomFeed.xml", setting);
                SyndicationFeed sf = SyndicationFeed.Load(xmlr);
                Assert.True(sf != null);

                // *** EXECUTE *** \\
                //Write the same feed that was read.
                XmlWriter xmlw = XmlWriter.Create(path);
                Atom10FeedFormatter atomFeed = new Atom10FeedFormatter(sf);
                atomFeed.WriteTo(xmlw);
                xmlw.Close();

                // *** VALIDATE *** \\
                Assert.True(File.Exists(path));
            }
            finally
            {
                // *** CLEANUP *** \\
                File.Delete(path);
            }
        }

        [Fact]
        public static void SyndicationFeed_Load_Write_Atom_Feed_()
        {
            string path = Path.GetTempFileName();

            try
            {
                // *** SETUP *** \\\
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                XmlReader xmlr = XmlReader.Create(@"atom_spec_example.xml", readerSettings);
                SyndicationFeed sf = SyndicationFeed.Load(xmlr);
                Assert.True(sf != null);

                // *** EXECUTE *** \\
                //Write the same feed that was read.
                XmlWriterSettings writerSettings = new XmlWriterSettings();

                XmlWriter xmlw = XmlWriter.Create(path, writerSettings);
                Atom10FeedFormatter atomFeed = new Atom10FeedFormatter(sf);
                atomFeed.WriteTo(xmlw);
                xmlw.Close();

                // *** VALIDATE *** \\
                Assert.True(File.Exists(path));
            }
            finally
            {
                // *** CLEANUP *** \\
                File.Delete(path);
            }
        }

        [Fact]
        public static void SyndicationFeed_Write_RSS_Atom()
        {
            string RssPath = Path.GetTempFileName();
            string AtomPath = Path.GetTempFileName();

            try
            {
                // *** SETUP *** \\
                SyndicationFeed feed = new SyndicationFeed("Contoso News", "<div>Most recent news from Contoso</div>", new Uri("http://www.Contoso.com/news"), "123FeedID", DateTime.Now);

                //Add an author
                SyndicationPerson author = new SyndicationPerson("jerry@Contoso.com");
                feed.Authors.Add(author);

                //Create item
                SyndicationItem item1 = new SyndicationItem("SyndicationFeed released for .net Core", "A lot of text describing the release of .net core feature", new Uri("http://Contoso.com/news/path"));

                //Add item to feed
                List<SyndicationItem> feedList = new List<SyndicationItem> { item1 };
                feed.Items = feedList;
                feed.ElementExtensions.Add("CustomElement", "", "asd");

                //add an image
                feed.ImageUrl = new Uri("http://2.bp.blogspot.com/-NA5Jb-64eUg/URx8CSdcj_I/AAAAAAAAAUo/eCx0irI0rq0/s1600/bg_Contoso_logo3-20120824073001907469-620x349.jpg");

                feed.BaseUri = new Uri("http://mypage.com");

                // Write to XML > rss
                XmlWriterSettings settings = new XmlWriterSettings();
                XmlWriter xmlwRss = XmlWriter.Create(RssPath, settings);
                Rss20FeedFormatter rssff = new Rss20FeedFormatter(feed);

                // Write to XML > atom

                XmlWriter xmlwAtom = XmlWriter.Create(AtomPath);
                Atom10FeedFormatter atomf = new Atom10FeedFormatter(feed);


                // *** EXECUTE *** \\
                rssff.WriteTo(xmlwRss);
                xmlwRss.Close();

                atomf.WriteTo(xmlwAtom); ;
                xmlwAtom.Close();

                // *** ASSERT *** \\
                Assert.True(File.Exists(RssPath));
                Assert.True(File.Exists(AtomPath));
            }
            finally
            {
                // *** CLEANUP *** \\
                File.Delete(RssPath);
                File.Delete(AtomPath);
            }
        }

        [Fact]
        public static void SyndicationFeed_Load_Rss()
        {
            XmlReaderSettings setting = new XmlReaderSettings();
            using (XmlReader reader = XmlReader.Create(@"rssSpecExample.xml", setting))
            {
                SyndicationFeed rss = SyndicationFeed.Load(reader);
                Assert.True(rss.Items != null);
            }
        }

        [Fact]
        public static void SyndicationFeed_Load_Atom()
        {
            XmlReaderSettings setting = new XmlReaderSettings();
            using (XmlReader reader = XmlReader.Create(@"atom_spec_example.xml", setting))
            {
                SyndicationFeed atom = SyndicationFeed.Load(reader);
                Assert.True(atom.Items != null);
            }
        }

        [Fact]
        [ActiveIssue(25080)]
        public static void SyndicationFeed_Rss_TestDisjointItems()
        {
            using (XmlReader reader = XmlReader.Create(@"RssDisjointItems.xml"))
            {
                // *** EXECUTE *** \\
                SyndicationFeed sf = SyndicationFeed.Load(reader);

                // *** ASSERT *** \\
                int count = 0;
                foreach (SyndicationItem item in sf.Items)
                {
                    count++;
                }

                Assert.True(count == 2);
            }
        }


        [Fact]
        [ActiveIssue(25080)]
        public static void SyndicationFeed_Atom_TestDisjointItems()
        {
            using (XmlReader reader = XmlReader.Create(@"AtomDisjointItems.xml"))
            {
                // *** EXECUTE *** \\
                SyndicationFeed sf = SyndicationFeed.Load(reader);

                // *** ASSERT *** \\
                int count = 0;
                foreach (SyndicationItem item in sf.Items)
                {
                    count++;
                }

                Assert.True(count == 2);
            }
        }

        [Fact]
        [ActiveIssue(25080)]
        public static void SyndicationFeed_Rss_WrongDateFormat()
        {
            // *** SETUP *** \\
            Rss20FeedFormatter rssformatter = new Rss20FeedFormatter();

            XmlReader reader = XmlReader.Create(@"rssSpecExampleWrongDateFormat.xml");

            // *** EXECUTE *** \\
            SyndicationFeed res = SyndicationFeed.Load(reader);

            // *** ASSERT *** \\
            Assert.True(!res.LastUpdatedTime.Equals(new DateTimeOffset()));
        }

        [Fact]
        public static void AtomEntryPositiveTest()
        {
            string file = @"brief-entry-noerror.xml";
            ReadWriteSyndicationItem(file, (itemObject) => new Atom10ItemFormatter(itemObject));
        }

        [Fact]
        public static void AtomEntryPositiveTest_write()
        {
            string file = @"AtomEntryTest.xml";
            string serializeFilePath = Path.GetTempFileName();
            bool toDeletedFile = true;

            SyndicationItem item = new SyndicationItem("SyndicationFeed released for .net Core", "A lot of text describing the release of .net core feature", new Uri("http://contoso.com/news/path"));
            item.Id = "uuid:43481a10-d881-40d1-adf2-99b438c57e21;id=1";
            item.LastUpdatedTime = new DateTimeOffset(Convert.ToDateTime("2017-10-11T11:25:55Z")).UtcDateTime;

            try
            {
                using (FileStream fileStream = new FileStream(serializeFilePath, FileMode.OpenOrCreate))
                {
                    using (XmlWriter writer = XmlDictionaryWriter.CreateTextWriter(fileStream))
                    {
                        Atom10ItemFormatter f = new Atom10ItemFormatter(item);
                        f.WriteTo(writer);
                    }
                }

                CompareHelper ch = new CompareHelper
                {
                    Diff = new XmlDiff()
                    {
                        Option = XmlDiffOption.IgnoreComments | XmlDiffOption.IgnorePrefix | XmlDiffOption.IgnoreWhitespace | XmlDiffOption.IgnoreChildOrder | XmlDiffOption.IgnoreAttributeOrder
                    }
                };

                string diffNode = string.Empty;
                if (!ch.Compare(file, serializeFilePath, out diffNode))
                {
                    toDeletedFile = false;
                    string errorMessage = $"The generated file was different from the baseline file:{Environment.NewLine}Baseline: {file}{Environment.NewLine}Actual: {serializeFilePath}{Environment.NewLine}Different Nodes:{Environment.NewLine}{diffNode}";
                    Assert.True(false, errorMessage);
                }
            }
            finally
            {
                if (toDeletedFile)
                {
                    File.Delete(serializeFilePath);
                }
            }
        }

        [Fact]
        public static void AtomFeedPositiveTest()
        {
            string dataFile = @"atom_feeds.dat";
            List<string> fileList = GetTestFilesForFeedTest(dataFile);
            List<AllowableDifference> allowableDifferences = GetAtomFeedPositiveTestAllowableDifferences();

            foreach (string file in fileList)
            {
                ReadWriteSyndicationFeed(file, (feedObject) => new Atom10FeedFormatter(feedObject), allowableDifferences);
            }
        }

        [Fact]
        public static void RssEntryPositiveTest()
        {
            string file = @"RssEntry.xml";
            ReadWriteSyndicationItem(file, (itemObject) => new Rss20ItemFormatter(itemObject));
        }

        [Fact]
        public static void RssFeedPositiveTest()
        {
            string dataFile = @"rss_feeds.dat";
            List<string> fileList = GetTestFilesForFeedTest(dataFile);
            List<AllowableDifference> allowableDifferences = GetRssFeedPositiveTestAllowableDifferences();

            foreach (string file in fileList)
            {
                ReadWriteSyndicationFeed(file, (feedObject) => new Rss20FeedFormatter(feedObject), allowableDifferences);
            }
        }

        private static void ReadWriteSyndicationItem(string file, Func<SyndicationItem, SyndicationItemFormatter> itemFormatter)
        {
            string serializeFilePath = Path.GetTempFileName();
            bool toDeletedFile = true;

            try
            {
                SyndicationItem itemObjct = null;
                using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    using (XmlReader reader = XmlDictionaryReader.CreateTextReader(fileStream, XmlDictionaryReaderQuotas.Max))
                    {
                        itemObjct = SyndicationItem.Load(reader);
                    }
                }

                using (FileStream fileStream = new FileStream(serializeFilePath, FileMode.OpenOrCreate))
                {
                    using (XmlWriter writer = XmlDictionaryWriter.CreateTextWriter(fileStream))
                    {
                        SyndicationItemFormatter formatter = itemFormatter(itemObjct);
                        formatter.WriteTo(writer);
                    }
                }

                // compare file filePath and serializeFilePath
                CompareHelper ch = new CompareHelper
                {
                    Diff = new XmlDiff()
                    {
                        Option = XmlDiffOption.IgnoreComments | XmlDiffOption.IgnorePrefix | XmlDiffOption.IgnoreWhitespace | XmlDiffOption.IgnoreChildOrder | XmlDiffOption.IgnoreAttributeOrder
                    }
                };

                string diffNode = string.Empty;
                if (!ch.Compare(file, serializeFilePath, out diffNode))
                {
                    toDeletedFile = false;
                    string errorMessage = $"The generated file was different from the baseline file:{Environment.NewLine}Baseline: {file}{Environment.NewLine}Actual: {serializeFilePath}{Environment.NewLine}Different Nodes:{Environment.NewLine}{diffNode}";
                    Assert.True(false, errorMessage);
                }
            }
            catch (Exception e)
            {
                Exception newEx = new Exception($"Failed File Name: {file}", e);
                throw newEx;
            }
            finally
            {
                if (toDeletedFile)
                {
                    File.Delete(serializeFilePath);
                }
            }
        }

        private static void ReadWriteSyndicationFeed(string file, Func<SyndicationFeed, SyndicationFeedFormatter> feedFormatter, List<AllowableDifference> allowableDifferences = null)
        {
            string serializeFilePath = Path.GetTempFileName();
            bool toDeletedFile = true;

            try
            {
                SyndicationFeed feedObjct;
                using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    using (XmlReader reader = XmlDictionaryReader.CreateTextReader(fileStream, XmlDictionaryReaderQuotas.Max))
                    {
                        feedObjct = SyndicationFeed.Load(reader);
                    }
                }

                using (FileStream fileStream = new FileStream(serializeFilePath, FileMode.OpenOrCreate))
                {
                    using (XmlWriter writer = XmlDictionaryWriter.CreateTextWriter(fileStream))
                    {
                        SyndicationFeedFormatter formatter = feedFormatter(feedObjct);
                        formatter.WriteTo(writer);
                    }
                }

                CompareHelper ch = new CompareHelper
                {
                    Diff = new XmlDiff()
                    {
                        Option = XmlDiffOption.IgnoreComments | XmlDiffOption.IgnorePrefix | XmlDiffOption.IgnoreWhitespace | XmlDiffOption.IgnoreChildOrder | XmlDiffOption.IgnoreAttributeOrder
                    },
                    AllowableDifferences = allowableDifferences
                };

                string diffNode = string.Empty;
                if (!ch.Compare(file, serializeFilePath, out diffNode))
                {
                    toDeletedFile = false;
                    string errorMessage = $"The generated file was different from the baseline file:{Environment.NewLine}Baseline: {file}{Environment.NewLine}Actual: {serializeFilePath}{Environment.NewLine}Different Nodes:{Environment.NewLine}{diffNode}";
                    Assert.True(false, errorMessage);
                }
            }
            catch (Exception e)
            {
                Exception newEx = new Exception($"Failed File Name: {file}", e);
                throw newEx;
            }
            finally
            {
                if (toDeletedFile)
                {
                    File.Delete(serializeFilePath);
                }
            }
        }

        private static List<AllowableDifference> GetAtomFeedPositiveTestAllowableDifferences()
        {
            return new List<AllowableDifference>(new AllowableDifference[]
            {
                new AllowableDifference("<content xmlns=\"http://www.w3.org/2005/Atom\" />","<content type=\"text\" xmlns=\"http://www.w3.org/2005/Atom\" />"),
                new AllowableDifference("<content>","<content type=\"text\">"),
                new AllowableDifference("<content src=\"http://example.org/2003/12/13/atom03\" xmlns=\"http://www.w3.org/2005/Atom\" />","<content src=\"http://example.org/2003/12/13/atom03\" type=\"text\" xmlns=\"http://www.w3.org/2005/Atom\" />"),
                new AllowableDifference("<content src=\"  http://www.example.com/doc.pdf\" type=\"application/pdf\" xmlns=\"http://www.w3.org/2005/Atom\" />","<content src=\"http://www.example.com/doc.pdf\" type=\"application/pdf\" xmlns=\"http://www.w3.org/2005/Atom\" />"),
                new AllowableDifference("<title>","<title type=\"text\">"),
                new AllowableDifference("<subtitle>","<subtitle type=\"text\">"),
                new AllowableDifference("<subtitle xmlns=\"http://www.w3.org/2005/Atom\" />","<subtitle type=\"text\" xmlns=\"http://www.w3.org/2005/Atom\" />"),
                new AllowableDifference("<title xmlns=\"http://www.w3.org/2005/Atom\" />","<title type=\"text\" xmlns=\"http://www.w3.org/2005/Atom\" />"),
                new AllowableDifference("<atom:title>","<title type=\"text\">"),
                new AllowableDifference("<summary>","<summary type=\"text\">"),
                new AllowableDifference("<atom:summary>","<summary type=\"text\">"),
                new AllowableDifference("<generator uri=\"http://www.example.com/\" version=\"1.0\">", "<generator>"),
                new AllowableDifference("<generator uri=\"/generator\" xmlns=\"http://www.w3.org/2005/Atom\" />", "<generator xmlns=\"http://www.w3.org/2005/Atom\" />"),
                new AllowableDifference("<generator uri=\"/generator\">", "<generator>"),
                new AllowableDifference("<generator uri=\"misc/Colophon\">", "<generator>"),
                new AllowableDifference("<generator uri=\"http://www.example.com/ \" version=\"1.0\">", "<generator>"),
                new AllowableDifference("<rights>","<rights type=\"text\">"),
                new AllowableDifference("<link href=\"http://example.com\" xmlns=\"http://www.w3.org/2005/Atom\" />","<link href=\"http://example.com/\" xmlns=\"http://www.w3.org/2005/Atom\" />"),
                new AllowableDifference("<link href=\"  http://example.org/  \" xmlns=\"http://www.w3.org/2005/Atom\" />","<link href=\"http://example.org/\" xmlns=\"http://www.w3.org/2005/Atom\" />"),
                new AllowableDifference("<feed xml:lang=\"\" xmlns=\"http://www.w3.org/2005/Atom\">","<feed xmlns=\"http://www.w3.org/2005/Atom\">"),
                new AllowableDifference("<entry xmlns:xh=\"http://www.w3.org/1999/xhtml\">","<entry>"),
                new AllowableDifference("<xh:div>","<xh:div xmlns:xh=\"http://www.w3.org/1999/xhtml\">"),
                new AllowableDifference("<summary type=\"xhtml\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\">","<summary type=\"xhtml\">"),
                new AllowableDifference("<xhtml:a href=\"http://example.com/\">","<xhtml:a href=\"http://example.com/\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\">"),
                new AllowableDifference("<feed xmlns:trackback=\"http://madskills.com/public/xml/rss/module/trackback/\" xmlns=\"http://www.w3.org/2005/Atom\">","<feed xmlns=\"http://www.w3.org/2005/Atom\">"),
                new AllowableDifference("<trackback:ping>", "<trackback:ping xmlns:trackback=\"http://madskills.com/public/xml/rss/module/trackback/\">"),
                new AllowableDifference("<feed xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:foaf=\"http://xmlns.com/foaf/0.1/\" xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" xmlns=\"http://www.w3.org/2005/Atom\">", "<feed xmlns=\"http://www.w3.org/2005/Atom\">"),
                new AllowableDifference("<author rdf:parseType=\"Resource\">", "<author xmlns:a=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" a:parseType=\"Resource\">"),
                new AllowableDifference("<foaf:homepage rdf:resource=\"http://jondoe.example.org/\">", "<foaf:homepage xmlns:foaf=\"http://xmlns.com/foaf/0.1/\" xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" rdf:resource=\"http://jondoe.example.org/\">"),
                new AllowableDifference("<foaf:weblog rdf:resource=\"http://jondoe.example.org/blog/\">", "<foaf:weblog xmlns:foaf=\"http://xmlns.com/foaf/0.1/\" xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" rdf:resource=\"http://jondoe.example.org/blog/\">"),
                new AllowableDifference("<foaf:workplaceHomepage rdf:resource=\"http://DoeCorp.example.com/\">", "<foaf:workplaceHomepage xmlns:foaf=\"http://xmlns.com/foaf/0.1/\" xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" rdf:resource=\"http://DoeCorp.example.com/\">"),
                new AllowableDifference("<dc:description>", "<dc:description xmlns:dc=\"http://purl.org/dc/elements/1.1/\">"),
                new AllowableDifference("<entry rdf:parseType=\"Resource\">", "<entry xmlns:a=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" a:parseType=\"Resource\">"),
                new AllowableDifference("<foaf:primaryTopic rdf:parseType=\"Resource\">", "<foaf:primaryTopic xmlns:foaf=\"http://xmlns.com/foaf/0.1/\" rdf:parseType=\"Resource\" xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\">"),
                new AllowableDifference("<link href=\"http://example.org/\" rdf:resource=\"http://example.org/\" xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" xmlns=\"http://www.w3.org/2005/Atom\" />", "<link xmlns:a=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" href=\"http://example.org/\" a:resource=\"http://example.org/\" xmlns=\"http://www.w3.org/2005/Atom\" />"),
                new AllowableDifference("<feed xmlns:creativeCommons=\"http://backend.userland.com/creativeCommonsRssModule\" xmlns=\"http://www.w3.org/2005/Atom\">", "<feed xmlns=\"http://www.w3.org/2005/Atom\">"),
                new AllowableDifference("<creativeCommons:license>", "<creativeCommons:license xmlns:creativeCommons=\"http://backend.userland.com/creativeCommonsRssModule\">"),
                new AllowableDifference("<feed xmlns:a=\"http://www.example.com/extension-a\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns=\"http://www.w3.org/2005/Atom\">", "<feed xmlns=\"http://www.w3.org/2005/Atom\">"),
                new AllowableDifference("<a:simple-value>", "<a:simple-value xmlns:a=\"http://www.example.com/extension-a\">"),
                new AllowableDifference("<a:structured-xml>", "<a:structured-xml xmlns:a=\"http://www.example.com/extension-a\">"),
                new AllowableDifference("<dc:title>", "<dc:title xmlns:dc=\"http://purl.org/dc/elements/1.1/\">"),
                new AllowableDifference("<simple-value>", "<simple-value xmlns=\"\">"),
                new AllowableDifference("<feed xmlns:xhtml=\"http://www.w3.org/1999/xhtml\" xmlns=\"http://www.w3.org/2005/Atom\">", "<feed xmlns=\"http://www.w3.org/2005/Atom\">"),
                new AllowableDifference("<trackback:about>", "<trackback:about xmlns:trackback=\"http://madskills.com/public/xml/rss/module/trackback/\">"),
                new AllowableDifference("<xhtml:img src=\"http://example.com/image.jpg\">", "<xhtml:img src=\"http://example.com/image.jpg\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\">"),
                new AllowableDifference("<feed xml:base=\"http://example.org\" xmlns=\"http://www.w3.org/2005/Atom\">", "<feed xml:base=\"http://example.org/\" xmlns=\"http://www.w3.org/2005/Atom\">"),
                new AllowableDifference("<link href=\"http://creativecommons.org/licenses/by-nc/2.5/\" xmlns:lic=\"http://web.resource.org/cc/\" xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" rel=\"http://www.snellspace.com/atom/extensions/proposed/license\" rdf:resource=\"http://creativecommons.org/licenses/by-nc/2.5/\" type=\"text/html\" rdf:type=\"http://web.resource.org/cc/license\">", "<link xmlns:a=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\" href=\"http://creativecommons.org/licenses/by-nc/2.5/\" rel=\"http://www.snellspace.com/atom/extensions/proposed/license\" a:resource=\"http://creativecommons.org/licenses/by-nc/2.5/\" type=\"text/html\" a:type=\"http://web.resource.org/cc/license\">"),
            });
        }

        private static List<AllowableDifference> GetRssFeedPositiveTestAllowableDifferences()
        {
            return new List<AllowableDifference>(new AllowableDifference[]
            {
                new AllowableDifference("<rss version=\"2.0\">","<rss xmlns:a10=\"http://www.w3.org/2005/Atom\" version=\"2.0\">"),
                new AllowableDifference("<content:encoded>", "<content:encoded xmlns:content=\"http://purl.org/rss/1.0/modules/content/\">"),
                new AllowableDifference("Tue, 31 Dec 2002 14:20:20 GMT", "Tue, 31 Dec 2002 14:20:20 Z"),
            });
        }

        private static List<string> GetTestFilesForFeedTest(string dataFile)
        {
            List<string> fileList = new List<string>();

            string file;
            using (StreamReader sr = new StreamReader(dataFile))
            {
                while (!string.IsNullOrEmpty(file = sr.ReadLine()))
                {
                    if (!file.StartsWith("#"))
                    {
                        file = file.Trim();
                        if (File.Exists(file))
                        {
                            fileList.Add(Path.GetFullPath(file));
                        }
                        else
                        {
                            throw new FileNotFoundException($"File `{file}` was not found!");
                        }
                    }
                }
            }
            return fileList;
        }

    }
}
