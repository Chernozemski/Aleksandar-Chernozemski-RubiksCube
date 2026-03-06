import {
  ThemeProvider,
  createTheme,
  CssBaseline,
  Box,
  Typography,
  Alert,
  Button,
} from "@mui/material";
import RefreshIcon from "@mui/icons-material/Refresh";
import { useCubeState } from "./cube/useCubeState";
import { ExplodedView } from "./cube/components/ExplodedView";
import { RotationButtons } from "./cube/components/RotationButtons";

const theme = createTheme({});

function App() {
  const { state, loading, error, applyMove, refetch } = useCubeState();

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Box sx={{ py: 2 }}>
        <Typography variant="h5" align="center">
          Rubik's Cube Rotator
        </Typography>
        {error && (
          <Alert severity="error" sx={{ m: 2 }}>
            {error}
          </Alert>
        )}
        {loading ? (
          <Typography align="center" sx={{ py: 4 }}>
            Loading...
          </Typography>
        ) : (
          <Box sx={{ py: 2 }}>
            <ExplodedView state={state} />
          </Box>
        )}
        <RotationButtons onMove={applyMove} disabled={loading || !state} />
        <Box sx={{ display: "flex", justifyContent: "center", mt: 2 }}>
          <Button
            variant="outlined"
            size="large"
            startIcon={<RefreshIcon sx={{ fontSize: 28 }} />}
            onClick={() => void refetch()}
            disabled={loading}
            sx={{ py: 1.5, px: 4 }}
          >
            Reset
          </Button>
        </Box>
      </Box>
    </ThemeProvider>
  );
}

export default App;
