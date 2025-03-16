using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    public void TestPriorityQueue_EnqueueDequeue_SingleItem()
    {
        var queue = new PriorityQueue();
        queue.Enqueue("Item1", 1);

        var result = queue.Dequeue();
        Assert.AreEqual("Item1", result);
    }

    [TestMethod]
    public void TestPriorityQueue_EnqueueDequeue_MultipleItems_DifferentPriorities()
    {
        var queue = new PriorityQueue();
        queue.Enqueue("Item1", 1);
        queue.Enqueue("Item2", 3);
        queue.Enqueue("Item3", 2);

        var result1 = queue.Dequeue();
        Assert.AreEqual("Item2", result1);

        var result2 = queue.Dequeue();
        Assert.AreEqual("Item3", result2);

        var result3 = queue.Dequeue();
        Assert.AreEqual("Item1", result3);
    }

    [TestMethod]
    public void TestPriorityQueue_EnqueueDequeue_MultipleItems_SameHighestPriority()
    {
        var queue = new PriorityQueue();
        queue.Enqueue("Item1", 2);
        queue.Enqueue("Item2", 2);
        queue.Enqueue("Item3", 1);

        var result1 = queue.Dequeue();
        Assert.AreEqual("Item1", result1);

        var result2 = queue.Dequeue();
        Assert.AreEqual("Item2", result2);

        var result3 = queue.Dequeue();
        Assert.AreEqual("Item3", result3);
    }

    [TestMethod]
    public void TestPriorityQueue_Dequeue_EmptyQueue()
    {
        var queue = new PriorityQueue();

        var exception = Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());
        Assert.AreEqual("The queue is empty.", exception.Message);
    }

    [TestMethod]
    public void TestPriorityQueue_ToString()
    {
        var queue = new PriorityQueue();
        queue.Enqueue("Item1", 1);
        queue.Enqueue("Item2", 2);

        var result = queue.ToString();
        Assert.AreEqual("[Item1 (Pri:1), Item2 (Pri:2)]", result);
    }

    [TestMethod]
    public void TestPriorityQueue_ToString_EmptyQueue()
    {
        var queue = new PriorityQueue();

        var result = queue.ToString();
        Assert.AreEqual("[]", result);
    }
}