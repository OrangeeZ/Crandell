using UnityEngine;
using System.Collections;
using System.Linq;

public static class TargetSelector {

    public static Character SelectTarget( Character currentCharacter, Vector3 direction ) {

        var planetTransform = currentCharacter.pawn.planetTransform;
        var characterToDirectionMap = Character.instances
            .Where( _ => _ != currentCharacter )
            .Select( _ => new {character = _, direction = Vector3.Dot( planetTransform.GetDirectionTo( _.pawn.position ), direction )} )
            .Where( _ => _.direction >= 0.85f )
            .ToList();

        characterToDirectionMap.Sort( ( a, b ) => ( b.character.pawn.position - currentCharacter.pawn.position ).magnitude.CompareTo( ( a.character.pawn.position - currentCharacter.pawn.position ).magnitude ) );
        characterToDirectionMap.Sort( ( a, b ) => a.direction.CompareTo( b.direction ) );

        return characterToDirectionMap.Any() ? characterToDirectionMap.First().character : null;
    }

}