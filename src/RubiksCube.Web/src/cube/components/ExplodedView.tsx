import { FaceGrid, FACE_GRID_SIZE } from "./FaceGrid";
import type { CubeState } from "../types";
import "./ExplodedView.scss";

function Placeholder() {
  return (
    <div
      className="exploded-view__placeholder"
      style={{ width: FACE_GRID_SIZE, height: FACE_GRID_SIZE }}
    />
  );
}

export function ExplodedView({ state }: { state: CubeState | null }) {
  if (!state) return null;

  return (
    <div className="exploded-view">
      <div className="exploded-row">
        <Placeholder />
        <FaceGrid cells={state.up} />
        <Placeholder />
        <Placeholder />
      </div>
      <div className="exploded-row">
        <FaceGrid cells={state.left} />
        <FaceGrid cells={state.front} />
        <FaceGrid cells={state.right} />
        <FaceGrid cells={state.back} />
      </div>
      <div className="exploded-row">
        <Placeholder />
        <FaceGrid cells={state.down} />
        <Placeholder />
        <Placeholder />
      </div>
    </div>
  );
}
