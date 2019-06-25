import React from "react";
import {
    Content,
    Text,
    Icon,
    Container,
    View, Badge
} from "native-base";
import {Image} from "react-native";
import {DrawerItemsProps, SectionList} from "react-navigation";
import Constants from 'expo-constants';
import {Journey} from "../../domain/types";
// @ts-ignore
import PersonImg from "../../assets/sadiqKhan.jpg"


interface Section {
    title: string,
    data: Journey[]
}

const myJourneys: Journey[] = [
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
        endName: "North Greenwich",
        startTime: "08:30"
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
        endName: "Heathrow",
        startTime: "08:30"
    }
];

const menuItems: Section[] = [
    {title: 'Pay', data: []},
    {title: 'Rewards', data: []},
    {title: 'Badges', data: []},
    {title: 'Places', data: myJourneys},
    {title: 'History', data: []},
    {title: 'Help', data: []},
    {title: 'Feedback', data: []},
    {title: 'Logout', data: []},
];

export const SideBar = (props: DrawerItemsProps) => {
    return (
        <Container style={{paddingTop: Constants.statusBarHeight}}>
            <Content>
                <View style={{
                    alignSelf: 'center',
                    flexDirection: 'row',
                    alignItems: 'center',
                }}>
                    <Image source={PersonImg} style={{width: 100, height: 100, borderRadius: 100, marginTop: 10}}/>
                    <View style={{
                        marginLeft: 15
                    }}>
                        <Text>Sadiq</Text>
                        <Text>Khan</Text>
                        <Badge success><Text>72</Text></Badge>
                    </View>
                </View>
                <SectionList style={{marginLeft: 15}}
                             sections={menuItems}
                             keyExtractor={((item, index) => item + index)}
                             renderItem={({item}) =>
                                 (
                                     <View style={{marginLeft: 10, padding: 10, flexDirection: 'row'}}>
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
                             renderSectionHeader=
                                 {({section: {title}}) => (
                                     <Text 
                                        style={{padding: 10, fontWeight: 'bold'}}
                                        onPress={() => {
                                            props.navigation.closeDrawer();
                                            props.navigation.navigate( title === "Help" ? "Help" : "Home");
                                        }}>
                                     {title}
                                     </Text>)}
                />
            </Content>
        </Container>
    )
};
