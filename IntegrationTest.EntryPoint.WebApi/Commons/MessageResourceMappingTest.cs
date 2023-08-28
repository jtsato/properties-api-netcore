using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Core.Commons.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTest.EntryPoint.WebApi.Commons;

[Collection("WebApi Collection [NoContext]")]
public sealed class MessageResourceMappingTest : IClassFixture<CoreMessageKeysFixture>
{
    private const string CurrentProjectName = "IntegrationTest.EntryPoint.WebApi";
    private const string MessageResourcePath = "EntryPoint.WebApi/Resources/";

    private const string DefaultMessageResourcePath = "Messages.resx";
    private const string EnUsMessageResourcePath = "Messages.en-US.resx";
    private const string PtBrMessageResourcePath = "Messages.pt-BR.resx";
    private readonly CoreMessageKeysFixture _coreMessageKeysFixture;

    private readonly ITestOutputHelper _outputHelper;

    public MessageResourceMappingTest(ITestOutputHelper outputHelper, CoreMessageKeysFixture coreMessageKeysFixture)
    {
        _outputHelper = outputHelper;
        _coreMessageKeysFixture = coreMessageKeysFixture;
    }

     [Trait("Category", "WebApi Collection [NoContext]")]
    [Theory(DisplayName = "Successful to validate messages mapped in message resources ")]
    [InlineData(DefaultMessageResourcePath)]
    [InlineData(EnUsMessageResourcePath)]
    [InlineData(PtBrMessageResourcePath)]
    public void SuccessfulToValidateMessagesMappedInMessageResources(string messageResource)
    {
        // Arrange
        string projectRootFolder = Directory.GetCurrentDirectory().SubstringBefore(CurrentProjectName);
        _outputHelper.WriteLine("The current directory is {0}", projectRootFolder);

        string messageResourcePath = $"{projectRootFolder}{MessageResourcePath}{messageResource}";
        List<string> messageResourcesKeys = GetMessageResourcesKeysByFilePath(messageResourcePath);

        // Act 
        IEnumerable<string> coreProjectMessageKeys = _coreMessageKeysFixture.GetCoreProjectMessageKeys();
        IEnumerable<string> messageKeysNotMappedInResourceFile =
            GetMessagesKeysNotMappedInResourceFile(messageResource, coreProjectMessageKeys, messageResourcesKeys);

        // Assert
        Assert.Empty(messageKeysNotMappedInResourceFile);
    }

    [ExcludeFromCodeCoverage]
    private IEnumerable<string> GetMessagesKeysNotMappedInResourceFile
    (
        string messageResource,
        IEnumerable<string> coreProjectMessageKeys,
        ICollection<string> messageResourcesKeys
    )
    {
        List<string> messageKeysNotMappedInResourceFile = new List<string>();
        IEnumerable<string> messageKeys = coreProjectMessageKeys.Where(messageKey => !messageResourcesKeys.Contains(messageKey));
        
        foreach (string messageKey in messageKeys)
        {
            messageKeysNotMappedInResourceFile.Add(messageKey);
            _outputHelper.WriteLine(
                $"The message key \"{messageKey}\" was not found in resource file {messageResource}");
        }

        return messageKeysNotMappedInResourceFile;
    }

    private static List<string> GetMessageResourcesKeysByFilePath(string messageResourcePath)
    {
        return (from line in File.ReadLines(messageResourcePath)
            where line.Contains("<data name=\"")
            select line.SubstringAfter("<data name=\"").SubstringBefore("\" xml:space")).ToList();
    }
}

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class CoreMessageKeysFixture : IDisposable
{
    private const string CurrentProjectName = "IntegrationTest.EntryPoint.WebApi";
    private readonly string _projectRootFolder;

    private List<string> _messageKeys;

    private bool _disposed;

    ~CoreMessageKeysFixture() => Dispose(false);

    public CoreMessageKeysFixture()
    {
        _projectRootFolder = Directory.GetCurrentDirectory().SubstringBefore(CurrentProjectName);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IEnumerable<string> GetCoreProjectMessageKeys()
    {
        return _messageKeys ??= LoadCoreProjectMessageKeys(_projectRootFolder);
    }

    private static List<string> LoadCoreProjectMessageKeys(string projectRootFolder)
    {
        string[] pathToFiles =
            Directory.GetFiles($"{projectRootFolder}Core/Domains", "*.cs", SearchOption.AllDirectories);

        List<string> messageKeys = (from pathToFile in pathToFiles
            from line in File.ReadLines(pathToFile)
            where line.Contains("Exception(\"") || line.Contains("WithMessage(\"")
            select line.Contains("Exception(\"")
                ? line.SubstringAfter("Exception(\"").SubstringBefore("\"")
                : line.SubstringAfter("WithMessage(\"").SubstringBefore("\"")).ToList();

        messageKeys.Sort();

        return messageKeys.Distinct().ToList();
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed || !disposing) return;
        _disposed = true;
    }
}