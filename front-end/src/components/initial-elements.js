import React from "react";
import { MarkerType, Position } from "reactflow";

export const initialNodes = [
  {
    id: "Rucoblud.Ssanina",
    type: "classNode",
    data: {
      name: "Ssanina",
      fullName: "Rucoblud.Ssanina",
      modifiers: "+",
      genericInfo: [],
      methods: [],
      members: [],
    },
  },
  {
    id: "penis.pizda.Zalupa<T>",
    type: "classNode",
    data: {
      name: "Zalupa",
      fullName: "penis.pizda.Zalupa<T>",
      modifiers: "+",
      genericInfo: ["T"],
      methods: ["+ fuck (ratio : int): void", "+ Dispose (): void"],
      members: [
        "- <<readonly>> _vagina? : penis.govno.Vagina1<T> = new()",
        "+ Prop : int<<get>><<private set>>",
      ],
    },
  },
  {
    id: "penis.pizda.IZalupa",
    type: "interfaceNode",
    data: {
      name: "IZalupa",
      fullName: "penis.pizda.IZalupa",
      modifiers: "<<internal>>",
      genericInfo: [],
      methods: ["- fuck (ratio : int): void"],
      members: [],
    },
  },
  {
    id: "penis.govno.VaginaEvent",
    type: "classNode",
    data: {
      name: "VaginaEvent",
      fullName: "penis.govno.VaginaEvent",
      modifiers: "+",
      genericInfo: [],
      methods: [],
      members: [],
    },
  },
  {
    id: "penis.govno.Vagina1<T>",
    type: "classNode",
    data: {
      name: "Vagina1",
      fullName: "penis.govno.Vagina1<T>",
      modifiers: "+",
      genericInfo: ["T"],
      methods: [],
      members: [],
    },
  },
  {
    id: "MainClass",
    type: "classNode",
    data: {
      name: "MainClass",
      fullName: "MainClass",
      modifiers: "+",
      genericInfo: [],
      methods: [
        "+ {static} TestMethod (): void",
        "# <<internal>> Main (args : string[]): void",
      ],
      members: [],
    },
  },
  {
    id: "penis.pizda.Zalupa<T>.ZalupaDelegate",
    type: "delegateNode",
    data: {
      name: "ZalupaDelegate",
      fullName: "penis.pizda.Zalupa<T>.ZalupaDelegate",
      modifiers: "+",
      returnType: "int",
      genericInfo: [],
      parameters: ["zalupa : int"],
    },
  },
  {
    id: "Research.NewEnum",
    type: "enumNode",
    data: {
      name: "NewEnum",
      fullName: "Research.NewEnum",
      modifiers: "+",
      members: ["Zalupa = 1", "Penis"],
    },
  },
  {
    id: "Research.CustomDelegate",
    type: "delegateNode",
    data: {
      name: "CustomDelegate",
      fullName: "Research.CustomDelegate",
      modifiers: "+",
      returnType: "int",
      genericInfo: [],
      parameters: ["z : int"],
    },
  },
];
export const initialEdges = [
  {
    id: "0",
    target: "Rucoblud.Ssanina",
    source: "penis.pizda.Zalupa<T>",
    type: "floating",
  },
  {
    id: "1",
    target: "penis.pizda.IZalupa",
    source: "penis.pizda.Zalupa<T>",
    type: "floating",
  },
  {
    id: "2",
    target: "penis.govno.Vagina1<T>",
    source: "penis.pizda.Zalupa<T>",
    type: "floating",
  },
];
