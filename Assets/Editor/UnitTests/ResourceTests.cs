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
        Assert.That(resource.ID, Is.EqualTo(DResource.NameToID(name)));
    }

    [Test]
    public void FactoryCreatesResourceFromNameAndAmount()
    {
        var name = "Test";
        var amount = 7;

        DResource resource = DResource.Create(name, amount);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(amount));
        Assert.That(resource.ID, Is.EqualTo(DResource.NameToID(name)));
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
        Assert.That(resource.ID, Is.EqualTo(DResource.NameToID(name)));
                
        DResource otherResource = DResource.Create(resource);

        Assert.That(otherResource.Name, Is.EqualTo(name));
        Assert.That(otherResource.Amount, Is.EqualTo(zero));
        Assert.That(otherResource.ID, Is.EqualTo(DResource.NameToID(name)));
    }

    [Test]
    public void NameToIDFound()
    {
        var name = "Test";
        var zero = 0;
        DResource resource = DResource.Create(name);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(zero));
        Assert.That(resource.ID, Is.EqualTo(DResource.NameToID(name)));

        Assert.DoesNotThrow(() => { DResource.NameToID(name); });
        Assert.That(DResource.NameToID(name), Is.EqualTo(resource.ID));
    }

    [Test]
    public void NameToIDNotFound()
    {
        var name = "Good";
        var wrongName = "Bad";
        var zero = 0;
        var exceptionMessage = "Resource name does not exist: Bad";

        DResource resource = DResource.Create(name);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(zero));
        Assert.That(resource.ID, Is.EqualTo(DResource.NameToID(name)));

        var notFoundException = Assert.Throws<ResourceNameNotFoundException>(() => { DResource.NameToID(wrongName); });
        Assert.That(notFoundException.Message, Is.EqualTo(exceptionMessage));
    }

}