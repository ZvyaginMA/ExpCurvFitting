namespace ExpCurvFitting.Application.TemplateGenerator
{
    public interface ITemplateGenerator
    {
        TemplateGenerator.Result Handle(TemplateGenerator.Command command);
    }
}