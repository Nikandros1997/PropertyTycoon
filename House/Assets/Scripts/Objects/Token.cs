using UnityEngine;

public class Token {

	private string shape;
	private int position;
	private bool passedGo;

	public Token (string shape) {
		position = 0;
		// change it before submission
		passedGo = false;
		this.shape = shape;
	}

	/**
     * Moves the token on the board.
     * 
     * @param position The amount of positions a token needs to be moved.
     * @return true if it has passed go, false otherwise.
     */
	public bool movePiece(int positions) {
		position += positions;

		if(position >= 40) {
			position -= 40;
			passedGo = true;
			return true;
		}
		return false;
	}

	/**
	 * Return whether the token has passed the go, or not.
	 */
	public bool HasPassedGo() {
		return passedGo;
	}

	/**
	 * Returns the name, shape of the Token.
	 */
	public string GetShape() {
		return shape;
	}

	/**
	 * Returns the position of the token on the board.
	 */
	public int GetPosition() {
		return position;
	}
}

