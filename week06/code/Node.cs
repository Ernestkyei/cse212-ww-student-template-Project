public class Node
{
    public int Data { get; set; }
    public Node? Right { get; private set; }
    public Node? Left { get; private set; }

    public Node(int data)
    {
        this.Data = data;
    }

    public void Insert(int value)
    {
        // TODO Start Problem 1
        if (value == Data){
            return;
        }


        if (value < Data)
        {
            // Insert to the left
            if (Left is null)
                Left = new Node(value);
            else
                Left.Insert(value);
        }
        else
        {
            // Insert to the right
            if (Right is null)
                Right = new Node(value);
            else
                Right.Insert(value);
        }
    }
    

    public bool Contains(int value)
    {
    // TODO Start Problem 2
    if (value == Data)
    {
        return true;
    }
    else if (value < Data)
    {
        return Left != null && Left.Contains(value);
    }
    else // value > Data
    {
        return Right != null && Right.Contains(value);
    }
    }




   public int GetHeight()
    {
    //TODO PROBLEM 4

    // Base case: If the node is null, return height as 0
    if (this == null) 
    {
        return 0;
    }

    // Recursively calculate the height of the left and right subtrees
    int leftHeight = Left != null ? Left.GetHeight() : 0;
    int rightHeight = Right != null ? Right.GetHeight() : 0;

    // Return the height of the current node: 1 + the max of left and right heights
    return 1 + Math.Max(leftHeight, rightHeight);
    }

}