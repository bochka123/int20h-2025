import js from '@eslint/js';
import globals from 'globals';
import reactHooks from 'eslint-plugin-react-hooks';
import reactRefresh from 'eslint-plugin-react-refresh';
import tseslint from 'typescript-eslint';
import simpleImportSort from 'eslint-plugin-simple-import-sort';
import sortExports from 'eslint-plugin-sort-exports';

export default tseslint.config(
    { ignores: ['dist', 'node_modules', 'vite.config.ts'] },
    {
        files: ['**/*.{ts,tsx}'],
        languageOptions: {
            ecmaVersion: 'latest',
            sourceType: 'module',
            globals: globals.browser,
            parser: tseslint.parser,
            parserOptions: {
                project: ['./tsconfig.app.json'], // Використовуємо тільки tsconfig.app.json
                tsconfigRootDir: process.cwd(),
            },
        },
        plugins: {
            'react-hooks': reactHooks,
            'react-refresh': reactRefresh,
            'simple-import-sort': simpleImportSort,
            'sort-exports': sortExports,
            '@typescript-eslint': tseslint.plugin,
        },
        rules: {
            ...js.configs.recommended.rules,
            ...tseslint.configs.recommended.rules,
            ...reactHooks.configs.recommended.rules,
            "no-unused-vars": [
                "error",
                {
                    "argsIgnorePattern": "^_",
                    "varsIgnorePattern": "^_",
                    "caughtErrorsIgnorePattern": "^_",
                }
            ],
            'react/jsx-props-no-spreading': 'off',
            'react/require-default-props': 'off',
            'react-refresh/only-export-components': [
                'warn',
                { allowConstantExport: true },
            ],
            '@typescript-eslint/no-non-null-assertion': 'off',
            'semi': ['error', 'always'],
            'object-curly-spacing': ['error', 'always', { 'arraysInObjects': false }],
            'simple-import-sort/imports': 'error',
            'sort-exports/sort-exports': ['error', { 'sortDir': 'asc' }],
            'eol-last': ['error', 'always'],
            'quotes': ['error', 'single'],
            '@typescript-eslint/explicit-function-return-type': [
                'error',
                {
                    allowExpressions: true,
                    allowTypedFunctionExpressions: true,
                    allowHigherOrderFunctions: true,
                },
            ],
            '@typescript-eslint/no-empty-function': 'off',
            '@typescript-eslint/no-misused-promises': 'off',
            '@typescript-eslint/no-empty-interface': 'off',
            'no-extra-boolean-cast': 'off',
            'no-console': ['error', { 'allow': ['warn', 'error'] }],
        },
    }
);
