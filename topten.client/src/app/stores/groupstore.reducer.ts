import { createReducer, on } from '@ngrx/store'
import { update } from './groupstore.actions'
import { GroupModel } from '../interfaces/groupmodel.model'
import { AppState } from '../app.state'

export const initialState: GroupModel = {} as GroupModel

export const groupReducer = createReducer(
  initialState,
  on(update,
    (state, { data }) => ({ ...data })
  )
)
