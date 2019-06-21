import React from "react";
import {
    Content,
    Text,
    Icon,
    Container,
    Left,
    View, Badge
} from "native-base";
import {DrawerItemsProps, SectionList} from "react-navigation";
import Constants from 'expo-constants';
import {Journey} from "../../domain/types";

interface Section {
    title: string,
    data: Journey[]
}

const myJourneys:Journey[] = [
    {
        id: 1,
        name: "Home to Work",
        icon: "business",
        start: {
            latitude: 51.4511732, longitude: -0.2138706
        },
        startName: "Westminster",
        end: {
            latitude: 51.5250836, longitude: -0.0769465
        },
        endName: "North Greenwich"
    },
    {
        id: 2,
        name: "Work to Heathrow",
        icon: "business",
        start: {
            latitude: 51.4511731, longitude: -0.2138706
        },
        startName: "North Greenwich",
        end: {
            latitude: 51.5250836, longitude: -0.0769465
        },
        endName: "Heathrow"
    },
    {
        id: 3,
        name: "Mum's",
        icon: "home",
        start: {
            latitude: 51.4511731, longitude: -0.2138706
        },
        startName: "Westminster",
        end: {
            latitude: 51.5250836, longitude: -0.0769465
        },
        endName: "Ealing"
    },
];

const menuItems : Section[] = [
    {title: 'Pay', data: []},
    {title: 'Rewards', data: []},
    {title: 'Badges', data: []},
    {title: 'Places', data: myJourneys},
    {title: 'History', data: []},
    {title: 'Help', data: []},
    {title: 'Feedback', data: []},
    {title: 'Logout', data: []},
]

export const SideBar = (props: DrawerItemsProps) => {
    return (
        <Container style={{paddingTop: Constants.statusBarHeight}}>
            <Content>
                <View style={{
                    alignSelf: 'center',
                    flexDirection: 'row',
                    alignItems: 'center',
                }}>
                    <Icon name={"md-person"}
                          style={
                              {
                                  fontSize: 96
                              }
                          }/>
                    <View style={{
                        marginLeft: 15
                    }}>
                        <Text>Jane</Text>
                        <Text>Public</Text>
                        <Badge success><Text>72</Text></Badge>
                    </View>
                </View>
                <SectionList style={{marginLeft: 15}}
                    sections = {menuItems}
                    renderItem = {({item}) => 
                    (
                        <View style={{marginLeft: 10, padding: 10, flexDirection: 'row'}} >
                            <Icon
                                onPress={() => {
                                    props.navigation.closeDrawer();
                                    props.navigation.navigate("Route",
                                        {journey: item});
                                }}
                                active
                                type={"MaterialIcons"}
                                name={item.icon}
                                style={{color: "#777", fontSize: 26, width: 30}}
                            />
                            <Text 
                                style={{paddingTop: 5}} 
                                onPress={() => {
                                    props.navigation.closeDrawer();
                                    props.navigation.navigate("Route",
                                        {journey: item});
                                }}>
                                {item.name}
                            </Text>
                        </View>
                        
                    )}
                    renderSectionHeader = 
                    {({section: {title}}) => (<Text style={{padding: 10, fontWeight: 'bold'}}>{title}</Text>)}
                />
            </Content>
        </Container>
    )
};
