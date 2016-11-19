using UnityEngine;
using System.Collections;

public class Bounty {
	
	public EnemyType enemyType { get; private set; }

	public Bounty (EnemyType enemyType) {
		this.enemyType = enemyType;
	}
}