export type RotationMove =
  | "F"
  | "F'"
  | "R"
  | "R'"
  | "U"
  | "U'"
  | "B"
  | "B'"
  | "L"
  | "L'"
  | "D"
  | "D'";

export type CubeState = {
  up: string[];
  down: string[];
  left: string[];
  right: string[];
  front: string[];
  back: string[];
};
