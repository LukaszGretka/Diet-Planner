import {User} from './user';

export interface SignInResult {
  user: User;
  returnUrl: string;
}
