using System.Collections;

public class CheckpointComparer : IComparer
{
    public int Compare(object x, object y)
    {
        return ((Checkpoint) x).level.CompareTo(((Checkpoint) y).level);
    }
}