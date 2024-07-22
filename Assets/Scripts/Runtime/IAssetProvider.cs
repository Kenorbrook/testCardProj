using TestGame;
using UnityEngine;

public interface IAssetProvider<T>
{
    T Instantiate(T obj, Transform at);
    GameObject Instantiate(GameObject obj, Transform at, bool instantiateInWorldSpace);
    T Instantiate(T obj);
}
public interface IAssetProviderForCharacter
    : IAssetProvider<CharacterObject>
{
}