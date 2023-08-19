import { User } from './user';

export interface SignUpResult {
  user: User;
  requireEmailConfirmation: boolean;
}
