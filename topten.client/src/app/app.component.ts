import { Component, OnInit } from '@angular/core'
import { SignalrService } from './services/signalr.service'
import { GroupModel } from './interfaces/groupmodel.model'
import { Store } from '@ngrx/store'
import { selectGroupId, selectPlayerList, selectState } from './stores/groupstore.selector'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  groupId$ = this.store.select(selectGroupId)
  state$ = this.store.select(selectState)
  constructor(public signalRService: SignalrService, private store: Store<GroupModel>) { }

  ngOnInit() {
    this.signalRService.startConnection()
    this.signalRService.updatedListener()
    this.signalRService.errorListener()
  }
}
