
using System;
using System.IO;
using System.Linq;
using FileSearcher.Search.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SearcherTest
{
    [TestClass]
    public class ConfigurationTest
    {
        /// <summary>
        ///     Adding a directory results in an added directory
        /// </summary>
        [TestMethod]
        public void AddDirectory()
        {
            SearchConfiguration configuration = new SearchConfiguration();
            string currentDirectory = Directory.GetCurrentDirectory();
            configuration.AddDirectory(currentDirectory);
            Assert.IsTrue(configuration.BaseDirectories.Contains(currentDirectory));
        }
        /// <summary>
        ///     Adding a directory that does not exist does not result in an added directory
        /// </summary>
        [TestMethod]
        public void AddBadDirectory()
        {
            SearchConfiguration configuration = new SearchConfiguration();
            string directory = "BadDir";
            configuration.AddDirectory(directory);
            Assert.IsFalse(configuration.BaseDirectories.Contains(directory));
        }

        /// <summary>
        ///     Adding one directory twice results in only one entry
        /// </summary>
        [TestMethod]
        public void AddSameDirectory()
        {
            SearchConfiguration configuration = new SearchConfiguration();
            string currentDirectory = Directory.GetCurrentDirectory();
            configuration.AddDirectory(currentDirectory);
            configuration.AddDirectory(currentDirectory);
            Assert.IsTrue(configuration.BaseDirectories.Contains(currentDirectory));
            Assert.IsTrue(configuration.BaseDirectories.Count == 1);
        }

        /// <summary>
        ///     Adding an extension results in an added extension
        /// </summary>
        [TestMethod]
        public void AddExtension()
        {
            SearchConfiguration configuration = new SearchConfiguration();
            string extension = ".txt";
            configuration.AddExtension(extension);
            Assert.IsTrue(configuration.Extensions.Contains(extension));
        }

        /// <summary>
        ///     Adding extensions with case differences and ignore case not chosen results in different entries
        /// </summary>
        [TestMethod]
        public void AddDifferentCaseExtensions()
        {
            SearchConfiguration configuration = new SearchConfiguration();
            string extension = ".txt";
            string capsExtension = ".Txt";
            configuration.AddExtension(extension, false);
            configuration.AddExtension(capsExtension, false);
            Assert.IsTrue(configuration.Extensions.Contains(extension));
            Assert.IsTrue(configuration.Extensions.Contains(capsExtension));
        }

        /// <summary>
        ///     Adding extensions with case differences and ignore case chosen results in one entry
        /// </summary>
        [TestMethod]
        public void AddDifferentCaseExtensionsIgnore()
        {
            SearchConfiguration configuration = new SearchConfiguration();
            string extension = ".txt";
            string capsExtension = ".Txt";
            configuration.AddExtension(extension);
            configuration.AddExtension(capsExtension);
            Assert.IsTrue(configuration.Extensions.Contains(extension));
            Assert.IsFalse(configuration.Extensions.Contains(capsExtension));
        }

        /// <summary>
        ///     Adding an key results in an added key
        /// </summary>
        [TestMethod]
        public void AddKey()
        {
            SearchConfiguration configuration = new SearchConfiguration();
            string key = "test";
            configuration.AddOrKey(key);
            SearchKey sKey = new SearchKey("test");
            Assert.IsTrue(configuration.OrKeys.Contains(sKey));
        }

        /// <summary>
        ///     Adding keys with case differences and ignore case not chosen results in different entries
        /// </summary>
        [TestMethod]
        public void AddDifferentCaseKeys()
        {
            SearchConfiguration configuration = new SearchConfiguration();
            string key = "test";
            string keyCase = "Test";
            configuration.AddOrKey(key, false);
            configuration.AddOrKey(keyCase, false);
            SearchKey sKey = new SearchKey("test", false);
            SearchKey sKeyCase = new SearchKey("Test", false);
            Assert.IsTrue(configuration.OrKeys.Contains(sKey));
            Assert.IsTrue(configuration.OrKeys.Contains(sKeyCase));
        }

        /// <summary>
        ///     Adding keys with case differences and ignore case chosen results in one entry
        /// </summary>
        [TestMethod]
        public void AddDifferentCaseKeysIgnore()
        {
            SearchConfiguration configuration = new SearchConfiguration();
            string key = "test";
            string keyCase = "Test";
            configuration.AddOrKey(key);
            configuration.AddOrKey(keyCase);
            Assert.IsTrue(configuration.OrKeys.Count() == 1);
        }

        /// <summary>
        ///     Json serialization and deserialization will result in the same object
        /// </summary>
        [TestMethod]
        public void JsonSerialization()
        {
            SearchConfiguration configuration = new SearchConfiguration();
            string key = "test";
            string extension = ".txt";
            configuration.AddOrKey(key);
            configuration.AddExtension(extension);

            string currentDirectory = Directory.GetCurrentDirectory();
            configuration.AddDirectory(currentDirectory);

            string json = configuration.ToJson();
            Assert.IsTrue(!string.IsNullOrEmpty(json));

            SearchConfiguration newConfig = SearchConfiguration.FromJson(json);
            Assert.IsTrue(newConfig != null);

            Assert.IsTrue(newConfig.OrKeys.First() == configuration.OrKeys.First());
            Assert.IsTrue(newConfig.Extensions.First() == configuration.Extensions.First());
            Assert.IsTrue(newConfig.BaseDirectories.First() == configuration.BaseDirectories.First());
        }
    }
}
