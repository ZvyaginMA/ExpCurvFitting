namespace ExpCurvFitting.Application.TemplateHeadersGenerator
{
    public interface ITemplateHeadersGenerator
    {
        TemplateHeadersGenerator.Result Handle(TemplateHeadersGenerator.Command command);
    }
}