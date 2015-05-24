namespace DecimalMath
{
    /// <summary>
    /// Interface that defines an object's ability to transform itself using a given matrix type.
    /// </summary>
    /// <typeparam name="TMatrix">Supported matrix type.</typeparam>
    /// <typeparam name="TSelf">Type that is implementing interface.</typeparam>
    public interface ITransformable<TMatrix, TSelf> where TMatrix : TransformationMatrixBase<TMatrix>, new()
    {
        TSelf Transform(TMatrix matrix);
    }
}