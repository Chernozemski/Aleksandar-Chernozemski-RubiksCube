import { Box } from "@mui/material";
import { FaceGrid } from "./FaceGrid";
import type { CubeState } from "../types";

export function Cube3DView({ state }: { state: CubeState | null }) {
  if (!state) return null;
  return (
    <Box>
      <Box>
        <FaceGrid cells={state.up} />
      </Box>
      <Box>
        <FaceGrid cells={state.left} />
        <FaceGrid cells={state.front} />
        <FaceGrid cells={state.right} />
        <FaceGrid cells={state.back} />
      </Box>
      <Box>
        <FaceGrid cells={state.down} />
      </Box>
    </Box>
  );
}
