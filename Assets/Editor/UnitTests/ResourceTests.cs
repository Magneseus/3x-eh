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

        Resource resource = Resource.Create(name);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(zero));
        Assert.That(resource.Id, Is.EqualTo(Resource.NameToId(name)));
    }

    [Test]
    public void FactoryCreatesResourceFromNameAndAmount()
    {
        var name = "Test";
        var amount = 7;

        Resource resource = Resource.Create(name, amount);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(amount));
        Assert.That(resource.Id, Is.EqualTo(Resource.NameToId(name)));
    }

    [Test]
    public void FactoryCreatesResourceFromResource()
    {
        var name = "Test";
        var amount = 7;
        var zero = 0;

        Resource resource = Resource.Create(name, amount);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(amount));
        Assert.That(resource.Id, Is.EqualTo(Resource.NameToId(name)));
                
        Resource otherResource = Resource.Create(resource);

        Assert.That(otherResource.Name, Is.EqualTo(name));
        Assert.That(otherResource.Amount, Is.EqualTo(zero));
        Assert.That(otherResource.Id, Is.EqualTo(Resource.NameToId(name)));
    }

    [Test]
    public void NameToIdFound()
    {
        var name = "Test";
        var zero = 0;
        Resource resource = Resource.Create(name);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(zero));
        Assert.That(resource.Id, Is.EqualTo(Resource.NameToId(name)));

        Assert.DoesNotThrow(() => { Resource.NameToId(name); });
        Assert.That(Resource.NameToId(name), Is.EqualTo(resource.Id));
    }

    [Test]
    public void NameToIdNotFound()
    {
        var name = "Good";
        var wrongName = "Bad";
        var zero = 0;
        var exceptionMessage = "Resource name does not exist: Bad";

        Resource resource = Resource.Create(name);

        Assert.That(resource.Name, Is.EqualTo(name));
        Assert.That(resource.Amount, Is.EqualTo(zero));
        Assert.That(resource.Id, Is.EqualTo(Resource.NameToId(name)));

        var notFoundException = Assert.Throws<ResourceNameNotFoundException>(() => { Resource.NameToId(wrongName); });
        Assert.That(notFoundException.Message, Is.EqualTo(exceptionMessage));
    }

}