import { createSlice } from '@reduxjs/toolkit';

import { RootState } from '@/store';

import { AuthState } from './types.ts';

const initialState: AuthState = {
    id: '',
};

const authSlice = createSlice({
    name: 'auth',
    initialState: initialState,
    reducers: {
        setProfile: (state, action) => {
            const { id } = action.payload as { id: string };
            state.id = id;
        },
        logOut: (state) => {
            state.id = initialState.id;
        }
    }
});

export const { setProfile, logOut } = authSlice.actions;
export default authSlice.reducer;
export const selectCurrentId = (state: RootState): string => state.auth.id;
