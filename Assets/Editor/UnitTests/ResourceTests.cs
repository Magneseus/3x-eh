using NUnit.Framework;
using NSubstitute;
using System;

public class ResourceTests
{

    [Test]
    public void ResourceTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void FactoryCreatesResourceFromName()
    {
        var name = "Test";
        var zero = 0;

        DResource resource = DResource.Create(name);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(zero));
        Assert.That(resource.Id, Is.EqualTo(DResource.NameToId(name)));
    }

    [Test]
    public void FactoryCreatesResourceFromNameAndAmount()
    {
        var name = "Test";
        var amount = 7;

        DResource resource = DResource.Create(name, amount);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(amount));
        Assert.That(resource.Id, Is.EqualTo(DResource.NameToId(name)));
    }

    [Test]
    public void FactoryCreatesResourceFromResource()
    {
        var name = "Test";
        var amount = 7;
        var zero = 0;

        DResource resource = DResource.Create(name, amount);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(amount));
        Assert.That(resource.Id, Is.EqualTo(DResource.NameToId(name)));
                
        DResource otherResource = DResource.Create(resource);

        Assert.That(otherResource.Name, Is.EqualTo(name));
        Assert.That(otherResource.Amount, Is.EqualTo(zero));
        Assert.That(otherResource.Id, Is.EqualTo(DResource.NameToId(name)));
    }

    [Test]
    public void NameToIdFound()
    {
        var name = "Test";
        var zero = 0;
        DResource resource = DResource.Create(name);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(zero));
        Assert.That(resource.Id, Is.EqualTo(DResource.NameToId(name)));

        Assert.DoesNotThrow(() => { DResource.NameToId(name); });
        Assert.That(DResource.NameToId(name), Is.EqualTo(resource.Id));
    }

    [Test]
    public void NameToIdNotFound()
    {
        var name = "Good";
        var wrongName = "Bad";
        var zero = 0;
        var exceptionMessage = "Resource name does not exist: Bad";

        DResource resource = DResource.Create(name);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(zero));
        Assert.That(resource.Id, Is.EqualTo(DResource.NameToId(name)));

        var notFoundException = Assert.Throws<ResourceNameNotFoundException>(() => { DResource.NameToId(wrongName); });
        Assert.That(notFoundException.Message, Is.EqualTo(exceptionMessage));
    }

}