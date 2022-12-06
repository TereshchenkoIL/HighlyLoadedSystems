using CoreLib.Models;

namespace CoreLib.Abstractions;

public interface IPoissonsRandomValuesGenerator
{
    PoissonsValuesResult Generate(GetRandomPoissonsRequestModel requestModel);
}