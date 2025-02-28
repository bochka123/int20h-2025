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
        // setProfile: (state, action) => {
        //     const { id, name, level, xp, logs } = action.payload as IProfileDto;
        //     state.id = id;
        //     state.name = name;
        //     state.level = level;
        //     state.xp = xp;
        //     state.logs = logs;
        // },
        // setUrl: (state, action) => {
        //     const { url } = action.payload as IProfileDto;
        //     state.url = url;
        // },
        // setLevel: (state, action) => {
        //     state.level = action.payload as IProfileLevelDto;
        // },
        logOut: (state) => {
            state.id = initialState.id;
        }
    }
});

export const { logOut } = authSlice.actions;
export default authSlice.reducer;
export const selectCurrentId = (state: RootState): string => state.auth.id;
