import { createAction, props } from '@ngrx/store';
import { Identity } from './identity'

export const getIdentitySuccess = createAction(
  '[Identity Service] Get identity success',
  props<{ identity: Identity }>()
);

export const signUpSuccess = createAction(
  '[Identity Service] Sign up success',
  props<{ identity: Identity }>()
);