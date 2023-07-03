using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class TokenCardInfo : CardInfo
{
    public TokenCardInfo(string name, CardSet set, CardType type, bool destroyOnDiscard = false) : base(name, set, type, -1, -1, destroyOnDiscard)
    {
    }

    public override object Clone()
    {
        return new TokenCardInfo(Name, Set, Type);
    }
}