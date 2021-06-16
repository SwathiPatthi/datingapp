import { ThrowStmt } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-lists',
  templateUrl: './member-lists.component.html',
  styleUrls: ['./member-lists.component.css']
})
export class MemberListsComponent implements OnInit {
  members$: Observable<Member[]>;

  constructor(private memberService : MembersService) { }

  ngOnInit(): void {
    this.members$ = this.memberService.getMembers();
  }

  // loadMembers(){
  //   this.memberService.getMembers().subscribe(members => {
  //     this.members = members;
  //   })
  // }

}
