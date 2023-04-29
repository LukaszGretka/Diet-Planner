import { createFeatureSelector } from '@ngrx/store';
import { AccountState } from './account.state';

const getState = createFeatureSelector<AccountState>('accountState');
