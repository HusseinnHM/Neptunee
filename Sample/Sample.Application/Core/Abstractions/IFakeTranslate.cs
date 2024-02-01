using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

namespace Sample.Application.Core.Abstractions;

public interface IFakeTranslate
{
    void Translate(params MultiLanguageProperty[] props);
}