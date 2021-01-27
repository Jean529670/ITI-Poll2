import { createReducer, on } from '@ngrx/store';
import { Identity } from './identity';
import { signUpSuccess } from './identity.service.actions';

export const initialState: Identity = {
  userId: 0,
  email: '',
  accessToken: ''
}

const _identityReducer = createReducer(
  initialState,
  on(signUpSuccess, (_, { identity }) => identity)
);

export function identityReducer(state, action) {
  return _identityReducer(state, action);
}