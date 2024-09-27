using Assets.Scripts.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlacedSave : Save
{

    public BaseCharacterSave[] characters;

    public PlacedSave(BaseCharacterSave[] characters)
    { this.characters = characters; }
}
