using System;
using System.Collections.Generic;

public class PriorityQueue
{
    // Internal list to store queue items
    private List<PriorityItem> _queue = new();

    /// <summary>
    /// Add a new value to the queue with an associated priority.  The
    /// node is always added to the back of the queue regardless of 
    /// the priority.
    /// </summary>
    /// <param name="value">The value</param>
    /// <param name="priority">The priority</param>
    public void Enqueue(string value, int priority)
    {
        var newNode = new PriorityItem(value, priority);
        _queue.Add(newNode); // Add to the end of the queue
    }

    /// <summary>
    /// Remove and return the item with the highest priority.
    /// If multiple items have the same priority, the one closest
    /// to the front of the queue is removed first.
    /// </summary>
    /// <returns>The value of the item removed</returns>
    /// <exception cref="InvalidOperationException">Thrown if the queue is empty</exception>
    public string Dequeue()
    {
        if (_queue.Count == 0) // Check if the queue is empty
        {
            throw new InvalidOperationException("The queue is empty.");
        }

        // Find the index of the item with the highest priority to remove
        var highPriorityIndex = 0;
        for (int index = 1; index < _queue.Count; index++) // Iterate through all items
        {
            if (_queue[index].Priority > _queue[highPriorityIndex].Priority)
            {
                highPriorityIndex = index; // Update the index of the highest priority item
            }
        }

        // Remove and return the item with the highest priority
        var value = _queue[highPriorityIndex].Value;
        _queue.RemoveAt(highPriorityIndex); // Remove the item from the queue
        return value;
    }

    /// <summary>
    /// Returns a string representation of the queue.
    /// </summary>
    /// <returns>A string representation of the queue</returns>
    public override string ToString()
    {
        return $"[{string.Join(", ", _queue)}]"; // Format the queue as a string
    }
}

internal class PriorityItem
{
    internal string Value { get; set; } // The value of the item
    internal int Priority { get; set; } // The priority of the item

    internal PriorityItem(string value, int priority)
    {
        Value = value;
        Priority = priority;
    }

    /// <summary>
    /// Returns a string representation of the priority item.
    /// </summary>
    /// <returns>A string representation of the priority item</returns>
    public override string ToString()
    {
        return $"{Value} (Pri:{Priority})"; // Format the item as a string
    }
}