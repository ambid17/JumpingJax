using System.Collections;

public class CheckpointComparer : IComparer
{
    public int Compare(object x, object y)
    {
        return (new CaseInsensitiveComparer()).Compare(((Checkpoint) x).level, ((Checkpoint) y).level);
    }
}