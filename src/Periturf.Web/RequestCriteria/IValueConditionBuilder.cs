namespace Periturf.Web.RequestCriteria
{
    public interface IValueConditionBuilder<T>
    {
        void AddNextValueEvaluatorSpecification(IValueEvaluatorSpecification<T> spec);
    }
}
