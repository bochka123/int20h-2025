import { configureStore } from '@reduxjs/toolkit';

import { apiSlice } from '@/services';

import authReducer from './auth/auth';

const store = configureStore({
    reducer: {
        [apiSlice.reducerPath]: apiSlice.reducer,
        auth: authReducer,
    },
    middleware: (getDefaultMiddleware) => 
        getDefaultMiddleware().concat(apiSlice.middleware),
});

type RootState = ReturnType<typeof store.getState>;

export { type RootState, store };
