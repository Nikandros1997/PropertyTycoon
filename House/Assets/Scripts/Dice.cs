using UnityEngine;

public class Dice {

	private int[] Die;
	private bool doneRolling;
	private bool doubles;

	/**
	 * Dice constructor.
	 */
	public Dice() {
		Die = new int[2];
		doneRolling = false;
		doubles = false;
	}

	/**
	 * Roll the dice.
	 */
	public int Roll() {

		doubles = false;

		Die[0] = Random.Range(1, 7);
		Die[1] = Random.Range(1, 7);

		if(Die[0] == Die[1]) {
			doubles = true;
		}

		doneRolling = true;

		return Die[0] + Die[1];
	}
	/**
	 * Returns if the current roll is doubles.
	 */
	public bool IsDoubles() {
		return doubles;
	}

	/**
	 * Attribute used for animations.
	 */
	public bool IsDoneRolling() {
		return doneRolling;
	}

	/**
	 * Get specific die.
	 * @die, int, number of die.
	 */
	public int GetDie(int die) {
		return Die [die];
	}

	/**
	 * Returns the two dice.
	 */
	public int[] GetRoll() {
		return Die;
	}
}
