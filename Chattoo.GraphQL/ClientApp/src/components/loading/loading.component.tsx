import { Box, CircularProgress, Container, LinearProgress, Paper, Stack, Typography } from '@mui/material';
import React from 'react'

interface LoadingProps {
    detail?: string
}

const Loading: React.FC<LoadingProps> = (props: LoadingProps) => {
    return (
        <Container maxWidth="xs">
            <Paper sx={{ p: 1}} elevation={0}>
                <Stack alignItems="center">
                    <Typography mb={2} variant="h5" textAlign="center">{props?.detail ? props.detail : "Načítání"}</Typography>
                    <Box sx={{position: "relative", height: "4em", width: "4em"}} mb={2}>
                        <CircularProgress color="success" size="4em" sx={{position: "absolute"}}/>
                        <CircularProgress color="primary" size="3.75em" sx={{position: "absolute"}}/>
                        <CircularProgress color="secondary" size="3.5em" sx={{position: "absolute"}}/>
                    </Box>
                </Stack>
            </Paper>
        </Container>
    );
}

export default Loading;