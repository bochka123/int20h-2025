import { FC, useState } from 'react';

import styles from './auth-page.module.scss';
import { AuthPageForm } from './auth-page-form.tsx';

const AuthPage: FC = () => {

    const [authType, setAuthType] = useState<'signIn' | 'signUp'>('signIn');
    
    return (
        <div className={styles.authContainer}>
            {/* <div className={styles.background} />
            <div className={styles.backgroundOverlay} /> */}
            <div className={styles.container}>
                <div className={styles.authCard}>
                    {authType === 'signIn' ? <h2>Sign In</h2> : <h2>Sign Up</h2>}
                    <AuthPageForm authType={authType}/>

                    {
                        authType === 'signIn' &&
                        <p className={styles.authFooter}>
                            Don't have an account? <a href="#" onClick={() => setAuthType('signUp')}>Sign Up</a>
                        </p>
                    }

                    {
                        authType === 'signUp' &&
                        <p className={styles.authFooter}>
                            Already have an account? <a href="#" onClick={() => setAuthType('signIn')}>Sign In</a>
                        </p>
                    }
                </div>
            </div>
        </div>
    );
};

export { AuthPage };
