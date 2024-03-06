import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-textinput',
  templateUrl: './textinput.component.html',
  styleUrl: './textinput.component.css'
})
export class TextinputComponent {
  @Input() placeholder: string = ''
  @Input() initialValue: string = ''
  @Output() valueChange = new EventEmitter<string>()

  onInputChange(event: any) {
    this.valueChange.emit(event.target.value)
  }
}
