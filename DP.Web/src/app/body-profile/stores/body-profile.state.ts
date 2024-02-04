import { Measurement } from '../models/measurement';
import { UserProfile } from '../models/user-profile';

export interface BodyProfileState {
  measurements: Measurement[];
  userProfile: UserProfile;
}
