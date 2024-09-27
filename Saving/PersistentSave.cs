using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PersistentSave : Save
{
    public SpawnedSave[] characters;

    public PersistentSave(SpawnedSave[] characters)
        { this.characters = characters; }
}
