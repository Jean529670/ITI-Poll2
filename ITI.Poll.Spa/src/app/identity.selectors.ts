import { createSelector } from "@ngrx/store";
import { AppState } from "./app.state";
import { Identity } from "./identity"

export const selectIsAuthenticated = createSelector(
  (state: AppState) => state.identity,
  (identity: Identity) => identity.accessToken != ''
);

export const selectCurrentEmail = createSelector(
  (state: AppState) => state.identity,
  (identity: Identity) => identity.email
);

export const selectAccessToken = createSelector(
  (state: AppState) => state.identity,
  (identity: Identity) => identity.accessToken
);