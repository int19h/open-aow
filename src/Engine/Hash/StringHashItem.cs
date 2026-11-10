
namespace Aow.Engine.Hash;

public struct StringHashItem
{
    private string key;

    public string Key { readonly get => key ?? ""; set => key = value; }

    public object Item;
}
