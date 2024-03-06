import { createAction, props } from '@ngrx/store'
import { GroupModel } from '../interfaces/groupmodel.model'

export const update = createAction('[GroupModel] Update', props<{data: GroupModel}>())
