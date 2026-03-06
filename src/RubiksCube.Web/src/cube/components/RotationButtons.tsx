import { Button, ButtonGroup } from "@mui/material";
import type { RotationMove } from "../types";
import "./RotationButtons.scss";

const NON_PRIME_MOVES: RotationMove[] = ["F", "R", "U", "B", "L", "D"];
const PRIME_MOVES: RotationMove[] = ["F'", "R'", "U'", "B'", "L'", "D'"];

export function RotationButtons({
  onMove,
  disabled,
}: {
  onMove: (move: RotationMove) => void;
  disabled?: boolean;
}) {
  return (
    <div className="rotation-buttons">
      <ButtonGroup variant="contained">
        {NON_PRIME_MOVES.map((move) => (
          <Button key={move} onClick={() => onMove(move)} disabled={disabled}>
            {move}
          </Button>
        ))}
      </ButtonGroup>
      <ButtonGroup variant="contained">
        {PRIME_MOVES.map((move) => (
          <Button key={move} onClick={() => onMove(move)} disabled={disabled}>
            {move}
          </Button>
        ))}
      </ButtonGroup>
    </div>
  );
}
