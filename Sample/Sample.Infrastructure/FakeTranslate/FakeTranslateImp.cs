using Neptunee.EntityFrameworkCore.MultiLanguage;
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;
using Sample.Application.Core.Abstractions;
using Sample.SharedKernel;

namespace Sample.Infrastructure.FakeTranslate;

public class FakeTranslateImp : IFakeTranslate
{
    public void Translate(params MultiLanguageProperty[] props)
    {
        foreach (var s in LanguageKeys.Other)
        {
            foreach (var prop in props)
            {
                prop.Upsert(s, $"{prop.GetFirst()} in {s}");
            }
        }
    }
}